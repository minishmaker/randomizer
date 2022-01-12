.thumb

push	{lr}
push	{r4-r7}

@ check if this was the golden enemy
mov	r7, r0 @ enemy data
ldrb	r0, [r7, #0x0A]
cmp	r0, #0x3C
blo	rupeelike
cmp	r0, #0x3E
bhi	rupeelike

	@ get the data we need
	ldr	r4, list
	ldr	r1, =#0x3000BF0
	ldrb	r5, [r1, #4] @ area
	ldrb	r6, [r1, #5] @ room
	@ for each entry in the list
	loop:
		@ check for the terminator
		ldr	r1, [r4]
		cmp	r1, #0
		beq	end
		@ check area
		ldrb	r1, [r4, #0]
		cmp	r1, r5
		bne	next
		@ check room
		ldrb	r1, [r4, #1]
		cmp	r1, r6
		beq	match
		@ go to next entry
		next:
		add	r4, #6
	b	loop

@ right area and room, spawn the item
match:
mov	r0, r7
ldrb	r1, [r4, #2]
ldrb	r2, [r4, #3]
ldr	r3, =#0x80542D4
mov	lr, r3
.short	0xF800

@ give the item the flag we want
ldrh	r0, [r4, #4]
mov	r2, #0x86
strh	r0, [r1, r2]
	
end:
pop	{r4-r7}
pop	{pc}

@ not the golden enemy, spawn normal item
rupeelike:
mov	r0, r7
mov	r1, #0x6E
ldrb	r1, [r7, r1]
mov	r2, #0x6F
ldrb	r2, [r7, r2]
ldr	r3, =#0x80542D4
mov	lr, r3
.short	0xF800
b	end

.align
.ltorg
list:
