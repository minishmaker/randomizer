.thumb
push	{r4-r7,lr}
mov	r4,r0	@number
mov	r5,r1	@x
mov	r6,r2	@y
ldr	r7,=#0x203F100
mov	r0,#0
strb	r0,[r7]
sub	r7,#1

@divide the number in a loop
numberLoop:
cmp	r4,#10
blo	endWrite
mov	r0,r4
mov	r1,#10
swi	#6
mov	r4,r0
divide:
mov	r0,r1
cmp	r0,#10
blo	write
mov	r1,#10
swi	#6
cmp	r0,#10
bhs	divide
write:
mov	r1,#0x30
add	r0,r1
strb	r0,[r7]
sub	r7,#1
b	numberLoop

endWrite:
mov	r0,#0x30
add	r0,r4
strb	r0,[r7]

flip:


endDraw:
mov	r0,r7
mov	r1,r5
mov	r2,r6
ldr	r3,drawText
mov	lr,r3
.short	0xF800
pop	{r4-r7,pc}
.align
.ltorg
drawText:
