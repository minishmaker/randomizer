.thumb
push	{r0-r3}
@check if we are ending
cmp	r1,#2
beq	end
cmp	r0,#0x1E
bhi	end

@check if this is bombs
cmp	r5,#7
blo	notBombs
cmp	r5,#8
bhi	notBombs

@check if we have bombs equipped
ldr	r0,=#0x2002AF4
ldrb	r1,[r0]
cmp	r1,#7
beq	end
cmp	r1,#8
beq	end
ldrb	r1,[r0,#1]
cmp	r1,#7
beq	end
cmp	r1,#8
beq	end
b	switch

@check if this is a bow
notBombs:
cmp	r5,#9
blo	notBow
cmp	r5,#10
bhi	notBow

@check if we have a bow equipped
ldr	r0,=#0x2002AF4
ldrb	r1,[r0]
cmp	r1,#9
beq	end
cmp	r1,#10
beq	end
ldrb	r1,[r0,#1]
cmp	r1,#9
beq	end
cmp	r1,#10
beq	end
b	switch

@check if this item is in any of the lists
notBow:
ldr	r0, list
listsloop:
	ldr	r1, [r0]
	cmp	r1, #0
	beq	switch
	mov	r2, r1
	itemsloop:
		ldrb	r3, [r2]
		cmp	r3, r5
		beq	listmatch
		add	r2, #1
		cmp	r3, #0xFF
	bne	itemsloop
	add	r0, #4
b	listsloop

@check if we have any item higher on the list equipped (or the same one)
listmatch:
ldrb	r3, [r2]
cmp	r3, #0xFF
beq	switch
	ldr	r0,=#0x2002AF4
	ldrb	r1,[r0, #0]
	cmp	r3, r1
	beq	end
	ldrb	r1,[r0, #1]
	cmp	r3, r1
	beq	end
add	r2, #1
b	listmatch

switch:
pop	{r0-r3}
mov	r0,r5
ldr	r3,=#0x8053FAC
mov	lr,r3
.short	0xF800
pop	{r4,r5,pc}

end:
pop	{r0-r3}
pop	{r4,r5,pc}
.align
.ltorg
list:
