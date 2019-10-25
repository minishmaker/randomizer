.thumb
push	{r0,r1}
@check if we are ending
cmp	r1,#2
beq	end
cmp	r0,#0x1E
bhi	end

@check if this is bombs
cmp	r5,#7
blo	switch
cmp	r5,#8
bhi	switch

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

switch:
pop	{r0,r1}
mov	r0,r5
ldr	r3,=#0x8053FAC
mov	lr,r3
.short	0xF800
pop	{r4,r5,pc}

end:
pop	{r0,r1}
pop	{r4,r5,pc}
