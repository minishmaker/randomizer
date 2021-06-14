.equ historyTable, drawText+4
.equ line, historyTable+4
.equ time, line+4
.thumb
ldrb	r0, [r6, #0x0A]
push	{r0-r7}

ldr	r4, =#0x203F300
mov	r5, #13
ldr	r6, time
ldr	r0, =#0xFFFF
cmp	r6, r0
blo	countLoop
	mov	r6, r0
countLoop:
	ldrh	r0, [r4, #0x00]
	cmp	r0, #0
	beq	countLoopNext
		ldrh	r0, [r4, #0x02]
		add	r0, #1
		cmp	r0, r6
		blo	countingNotDone
			mov	r0, #0
			strh	r0, [r4, #0x00]
		countingNotDone:
		strh	r0, [r4, #0x02]
	countLoopNext:
	add	r4, #4
	sub	r5, #1
bne	countLoop

ldr	r4, =#0x203F300
mov	r5, #13
ldr	r6, =#0x2035132
ldr	r0, line
lsl	r0, #6
sub	r6, r0
ldr	r7, historyTable
drawLoop:
	ldrh	r0, [r4]
	cmp	r0, #0
	beq	clean
	lsl	r0, #2
	ldr	r0, [r7, r0]
	mov	r3, r6
	ldr	r2, drawText
	mov	lr, r2
	.short	0xF800
	b	next
	clean:
		mov	r0, #0
		mov	r1, #0x10
		mov	r2, #0
		cleanloop:
			str	r0, [r6, r2]
			add	r2, #4
			sub	r1, #1
		bne	cleanloop
	next:
	add	r4, #4
	sub	r6, #0x40
	sub	r5, #1
bne	drawLoop

end:
@set bg0 to update
ldr	r0,=#0x3000F5E
mov	r1,#1
strh	r1,[r0]
pop	{r0-r7}
ldrb	r0, [r6, #0x0A]
cmp	r0, #0
bne	return2
return1:
ldr	r3,=#0x801C609
bx	r3
return2:
ldr	r3,=#0x801C501
bx	r3

.ltorg
.align
.align
.ltorg
drawText:
@POIN drawText
@POIN historyTable
@WORD line
@WORD time
