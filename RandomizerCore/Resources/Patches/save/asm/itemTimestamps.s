.equ table, offset+4
.thumb
push	{r4,lr}
push	{r0-r7}
cmp	r0,#0x7F
bhi	end
ldr	r1,table
ldrb	r0,[r1,r0]
cmp	r0,#0xFF
beq	end
lsl	r0,#2
ldr	r1,offset
ldr	r2,[r1,r0]
cmp	r2,#0
bne	end
ldr	r2,=#0x203FFF0
ldr	r2,[r2]
str	r2,[r1,r0]
end:
ldr	r3,=#0x807C4CC
mov	lr,r3
pop	{r0-r7}
mov	r3,r0
lsr	r4,r3,#2
ldr	r0,=#0x2002B32
.short	0xF800
.align
.ltorg
offset:
